import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  complexPassword = new RegExp('^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*\\W).+$');

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router
  ) {}

  registerForm = this.fb.group({
    displayName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: [
      '',
      Validators.required,
      Validators.pattern(this.complexPassword),
    ],
  });

  onSubmit() {
    this.accountService
      .register(this.registerForm.value)
      .then(() => this.router.navigateByUrl('/shop'));
  }
}
