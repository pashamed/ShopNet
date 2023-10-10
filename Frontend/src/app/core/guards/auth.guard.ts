import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from 'src/app/account/account.service';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const router = inject(Router);
  return accountService.user
    ? true
    : router
        .navigate(['/account/login'], {
          queryParams: { returnUrl: state.url },
        })
        .finally(() => {
          return false;
        });
};
