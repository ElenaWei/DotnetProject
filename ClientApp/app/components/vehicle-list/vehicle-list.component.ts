import { VehicleService } from './../../services/vehicle.service';
import { Component, OnInit } from '@angular/core';
import { Vehicle } from '../../models/vehicle';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css']
})
export class VehicleListComponent implements OnInit {

  vehicles: Vehicle[] | undefined;

  constructor(private vehicleService: VehicleService) { }

  ngOnInit() {
    this.vehicleService.getVehicles()
    .subscribe(v => this.vehicles = v);
  }

}
