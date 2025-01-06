import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ButtonCustomComponent } from "./components/button-custom/button-custom.component";
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { RouterModule } from "@angular/router";
import { CardComponent } from './components/card/card.component';
import { GoBackComponent } from './components/go-back/go-back.component';
import { TransactionComponent } from './components/transaction/transaction.component';
import { LineChartComponent } from './components/line-chart/line-chart.component';
import { MiniNavComponent } from './components/mini-nav/mini-nav.component';
import { ChartPieComponent } from "./components/chart-pie/chart-pie.component";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { TransformCardNumberPipe } from "./pipes/format-cardNumber.pipe";
import { TranslateModule } from "@ngx-translate/core";
import { FormatPricePipe } from "./pipes/format-price.pipe";
import { DateFormatterPipe } from "./pipes/dateFormatter.pipe";

@NgModule({
    declarations: [
        ButtonCustomComponent,
        MainLayoutComponent,
        CardComponent,
        GoBackComponent,
        TransactionComponent,
        LineChartComponent,
        MiniNavComponent,
        ChartPieComponent,
        TransformCardNumberPipe,
        FormatPricePipe,
        DateFormatterPipe
    ],
    imports: [
        CommonModule,
        RouterModule,
        NgbModule, 
        TranslateModule
    ],
    exports: [
        CommonModule, ButtonCustomComponent, CardComponent, 
        GoBackComponent, TransactionComponent, LineChartComponent,
        MiniNavComponent, ChartPieComponent, TransformCardNumberPipe,
        TranslateModule, FormatPricePipe, DateFormatterPipe
    ]
})
export class SharedModule { }
