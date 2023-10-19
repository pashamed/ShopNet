import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { AccountService } from 'src/app/account/account.service';
import { Address } from 'src/app/shared/models/user';

@Component({
  selector: 'app-checkout-address',
  templateUrl: './checkout-address.component.html',
  styleUrls: ['./checkout-address.component.scss'],
})
export class CheckoutAddressComponent {
  @Input() checkoutForm?: FormGroup;
  constructor(private accountService: AccountService) {}

  async saveUserAddress() {
    await this.accountService.updateUserAddress(
      this.checkoutForm?.get('addressForm')?.value
    );
    //to disable saveDefault button
    this.checkoutForm?.get('addressForm')?.setErrors({ street: true });
  }
}
