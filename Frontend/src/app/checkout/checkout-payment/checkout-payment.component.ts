import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BasketService } from 'src/app/basket/basket.service';
import { CheckoutService } from '../checkout.service';
import { ToastrService } from 'ngx-toastr';
import { Basket } from 'src/app/shared/models/basket';
import { DeliveryMethod } from 'src/app/shared/models/deliveryMethod';
import { Address } from 'src/app/shared/models/user';
import { NavigationExtras, Router } from '@angular/router';
import {
  Stripe,
  StripeCardCvcElement,
  StripeCardCvcElementChangeEvent,
  StripeCardExpiryElement,
  StripeCardExpiryElementChangeEvent,
  StripeCardNumberElement,
  StripeCardNumberElementChangeEvent,
  loadStripe,
} from '@stripe/stripe-js';
import { environment } from 'src/environments/environment.development';
import { OrderToCreate } from 'src/app/shared/models/order';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss'],
})
export class CheckoutPaymentComponent implements OnInit {
  @Input() checkoutForm?: FormGroup;
  @ViewChild('cardNumber') cardNumberElement?: ElementRef;
  @ViewChild('cardExpiry') cardExpiryElement?: ElementRef;
  @ViewChild('cardCVC') cardCvcElement?: ElementRef;
  stripe: Stripe | null = null;
  cardNumber?: StripeCardNumberElement;
  cardExpiry?: StripeCardExpiryElement;
  cardCvc?: StripeCardCvcElement;
  cardErrors: string | null = null;
  cardNumberOk = false;
  cardExpiryOk = false;
  cardCvcOk = false;
  loading = false;

  constructor(
    private basketService: BasketService,
    private checkoutService: CheckoutService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    console.log(this.cardErrors);
    this.initStripe();
  }

  async submitOrder() {
    this.loading = true;
    const basket = await this.basketService.getCurrentBasketValue();
    try {
      const createdOrder = await this.createOrder(basket);
      const paymentResult = await this.confirmPaymentWithStripe(basket);
      if (paymentResult.paymentIntent) {
        this.basketService.deleteBasket(basket!);
        const navigationExtras: NavigationExtras = { state: createdOrder };
        this.router.navigate(['checkout/success'], navigationExtras);
      } else {
        this.toastr.error(paymentResult.error.message);
      }
    } catch (error: any) {
      console.error(error);
      this.toastr.error(error.message);
    } finally {
      this.loading = false;
    }
  }

  private async confirmPaymentWithStripe(basket: Basket | null) {
    if (!basket) throw new Error('Basket is null');
    const result = await this.stripe
      ?.confirmCardPayment(basket.clientSecret!, {
        payment_method: {
          card: this.cardNumber!,
          billing_details: {
            name: this.checkoutForm?.get('paymentForm')?.get('nameOnCard')
              ?.value,
          },
        },
      })
      .catch((error: string) => {
        throw new Error(error);
      });
    if (!result) throw new Error('Problem with payment');
    return result;
  }

  private async createOrder(basket: Basket | null) {
    if (!basket) throw new Error('Basket is null');
    const orderToCreate = this.getOrderToCreate(basket);
    return await this.checkoutService.createOrder(orderToCreate);
  }

  getOrderToCreate(basket: Basket): OrderToCreate {
    const deliveryMethodId = this.checkoutForm
      ?.get('deliveryForm')
      ?.get('deliveryMethod')?.value as number;
    const shipToAddress: Address = this.checkoutForm?.get('addressForm')?.value;

    if (!deliveryMethodId || !shipToAddress)
      throw new Error('Problem with basket');
    return {
      basketId: basket.id,
      deliveryMethodId: deliveryMethodId,
      shipToAddress: shipToAddress,
    };
  }

  async initStripe() {
    this.stripe = await loadStripe(environment.stripePK);
    const elements = this.stripe?.elements();
    if (elements) {
      this.cardNumber = elements.create('cardNumber');
      this.cardNumber.mount(this.cardNumberElement?.nativeElement);
      this.cardNumber.on(
        'change',
        (event: StripeCardNumberElementChangeEvent) => {
          if (event.error) this.cardErrors = event.error.message;
          if (event.complete) this.cardNumberOk = true;
          this.cardErrors = null;
        }
      );

      this.cardExpiry = elements.create('cardExpiry');
      this.cardExpiry.mount(this.cardExpiryElement?.nativeElement);
      this.cardExpiry.on(
        'change',
        (event: StripeCardExpiryElementChangeEvent) => {
          if (event.error) this.cardErrors = event.error.message;
          if (event.complete) this.cardExpiryOk = true;
          this.cardErrors = null;
        }
      );

      this.cardCvc = elements.create('cardCvc');
      this.cardCvc.mount(this.cardCvcElement?.nativeElement);
      this.cardCvc.on('change', (event: StripeCardCvcElementChangeEvent) => {
        if (event.error) this.cardErrors = event.error.message;
        if (event.complete) this.cardCvcOk = true;
        this.cardErrors = null;
      });
    }
  }

  paymentFormOk() {
    return (
      this.cardNumberOk &&
      this.cardCvcOk &&
      this.cardExpiryOk &&
      this.checkoutForm?.get('paymentForm')?.valid
    );
  }
}
