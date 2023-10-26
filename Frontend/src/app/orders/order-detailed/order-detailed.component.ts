import { Component, Input, OnInit } from '@angular/core';
import { Order } from 'src/app/shared/models/order';
import { OrdersService } from '../orders.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-order-detailed',
  templateUrl: './order-detailed.component.html',
  styleUrls: ['./order-detailed.component.scss'],
})
export class OrderDetailedComponent implements OnInit {
  order: Order | undefined = undefined;

  constructor(
    private orderService: OrdersService,
    private route: ActivatedRoute,
    private breadCrumbService: BreadcrumbService
  ) {}
  ngOnInit(): void {
    this.getOrder(<string>this.route.snapshot.paramMap.get('id'));
  }
  async getOrder(id: string) {
    this.order = await this.orderService.getOrder(id);
    this.breadCrumbService.set(
      'orders/:id',
      `Order #${this.order.id}  ${this.order.status}`
    );
  }
}
