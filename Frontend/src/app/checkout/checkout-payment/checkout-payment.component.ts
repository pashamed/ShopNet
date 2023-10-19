import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BasketService } from 'src/app/basket/basket.service';
import { CheckoutService } from '../checkout.service';
import { ToastrService } from 'ngx-toastr';
import { Basket } from 'src/app/shared/models/basket';
import { DeliveryMethod } from 'src/app/shared/models/deliveryMethod';
import { Address } from 'src/app/shared/models/user';
import { NavigationExtras, Router } from '@angular/router';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss'],
})
export class CheckoutPaymentComponent {
  @Input() checkoutForm?: FormGroup;

  constructor(
    private basketService: BasketService,
    private checkoutService: CheckoutService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  submitOrder() {
    const basket = this.basketService.getCurrentBasketValue();
    if (!basket) return;
    const orderToCreate = this.getOrderToCreate(basket);
    if (!orderToCreate) return;
    this.checkoutService.createOrder(orderToCreate).then((order) => {
      this.toastr.success('Order created successfully');
      this.basketService.deleteBasket(basket);
      const navigationExtras: NavigationExtras = { state: order };
      this.router.navigate(['checkout/success'], navigationExtras);
    });
  }
  getOrderToCreate(basket: Basket) {
    const deliveryMethodId = this.checkoutForm
      ?.get('deliveryForm')
      ?.get('deliveryMethod')?.value as number;
    const shipToAddress: Address = this.checkoutForm?.get('addressForm')?.value;

    if (!deliveryMethodId || !shipToAddress) return;
    return {
      basketId: basket.id,
      deliveryMethodId: deliveryMethodId,
      shipToAddress: shipToAddress,
    };
  }
}
