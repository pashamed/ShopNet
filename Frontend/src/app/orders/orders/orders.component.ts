import { Component, OnInit, Output } from '@angular/core';
import { OrdersService } from '../orders.service';
import { Order } from 'src/app/shared/models/order';
import { outputAst } from '@angular/compiler';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss'],
})
export class OrdersComponent implements OnInit {
  orders: Order[] = [];

  constructor(private orderService: OrdersService) {}
  ngOnInit(): void {
    this.orderService.getOrders().subscribe((ord) => (this.orders = ord));
  }
}
