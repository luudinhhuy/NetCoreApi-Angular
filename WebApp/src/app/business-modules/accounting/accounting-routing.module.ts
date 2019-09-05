import { Routes, RouterModule } from '@angular/router';
import { AccountReceivablePayableComponent } from './account-receivable-payable/account-receivable-payable.component';
import { ModuleWithProviders } from '@angular/compiler/src/core';

const routes: Routes = [
  {
    path: '', redirectTo: 'statement-of-account'
  },
  {
    path: 'statement-of-account', loadChildren: () => import('./statement-of-account/statement-of-account.module').then(m => m.StatementOfAccountModule),
  },
  {
    path: 'advance-payment', loadChildren: () => import('./advance-payment/advance-payment.module').then(m => m.AdvancePaymentModule),
  },
  {
    path: 'settlement-payment', loadChildren: () => import('./settlement-payment/settlement-payment.module').then(m => m.SettlementPaymentModule),
  },
  {
    path: 'account-receivable-payable', component: AccountReceivablePayableComponent, data: {
      name: "Account Receivable Payable",
      level: 2
    }
  },
  // TODO another MODULE...
];

export const routing: ModuleWithProviders = RouterModule.forChild(routes);
