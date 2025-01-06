import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { MainLayoutComponent } from './shared/layouts/main-layout/main-layout.component';
import { AuthGuard } from './sign-in/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: '/intro', pathMatch: 'full' }, // Redirecționare implicită la /intro
  {
    path: 'intro',
    loadChildren: () => import('./intro/intro.module')
      .then(m => m.IntroModule)
  },
 
  {
    path: '',
    canActivateChild: [AuthGuard], 
    children: [ 
      {
        path: 'sign-in',
        loadChildren: () => import('./sign-in/sign-in.module')
          .then(m => m.SignInModule)
      },
      {
        path: 'profile',
        loadChildren: () => import('./pages/profile/profile.module')
          .then(m => m.ProfileModule)
      },
      {
        path: 'notifications',
        loadChildren: () => import('./pages/notifications/notifications.module')
          .then(m => m.NotificationsModule)
      },
      {
        path: 'edit-profile',
        loadChildren: () => import('./pages/edit-profile/edit-profile.module')
          .then(m => m.EditProfileModule)
      },
      {
        path: 'add-card',
        loadChildren: () => import('./pages/add-card/add-card.module')
          .then(m => m.AddCardModule)
      },
      {
        path: 'all-cards',
        loadChildren: () => import('./pages/all-cards/all-cards.module')
          .then(m => m.AllCardsModule)
      },
      {
        path: 'search',
        loadChildren: () => import('./pages/search/search.module')
          .then(m => m.SearchModule)
      },
      {
        path: 'send-money',
        loadChildren: () => import('./pages/send-money/send-money.module')
          .then(m => m.SendMoneyModule)
      },
      {
        path: 'request-money',
        loadChildren: () => import('./pages/request-money/request-money.module')
          .then(m => m.RequestMoneyModule)
      },
      {
        path: 'language',
        loadChildren: () => import('./pages/language/language.module')
          .then(m => m.LanguageModule)
      },
      {
        path: 'conditions',
        loadChildren: () => import('./pages/conditions/conditions.module')
          .then(m => m.ContidionsModule)
      },
      {
        path: 'change-password',
        loadChildren: () => import('./pages/change-password/change-password.module')
          .then(m => m.ChangePasswordModule)
      },
      {
        path: '',
        component: MainLayoutComponent, // Rutele de sub MainLayoutComponent
        children: [
          {
            path: 'home',
            loadChildren: () => import('./pages/home/home.module')
              .then(m => m.HomeModule)
          },
          {
            path: 'transfers',
            loadChildren: () => import('./pages/transfers/transfers.module')
              .then(m => m.TransfersModule)
          },
          {
            path: 'my-cards',
            loadChildren: () => import('./pages/my-cards/my-cards.module')
              .then(m => m.MyCardsModule)
          },
          {
            path: 'statistics',
            loadChildren: () => import('./pages/statistics/statistics.module')
              .then(m => m.StatisticsModule)
          },
          {
            path: 'settings',
            loadChildren: () => import('./pages/settings/settings.module')
              .then(m => m.SettingsModule)
          },
          {
            path: 'transaction-history',
            loadChildren: () => import('./pages/transaction-history/transaction-history.module')
              .then(m => m.TransactionHistoryModule)
          },
        ]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }