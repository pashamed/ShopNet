import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from 'src/app/account/account.service';

export const authGuard: CanActivateFn = async (route, state) => {
  const accountService = inject(AccountService);
  const router = inject(Router);
  if (accountService.user) {
    return true;
  }
  await router.navigate(['/account/login'], {
    queryParams: { returnUrl: state.url },
  });
  return false;
};
