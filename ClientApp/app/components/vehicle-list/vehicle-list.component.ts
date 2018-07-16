import { PaginationComponent } from './../shared/pagination/pagination.component';
import { KeyValuePair, Vehicle } from './../../models/vehicle';
import { VehicleService } from './../../services/vehicle.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css']
})
export class VehicleListComponent implements OnInit {

  private readonly PAGE_SIZE = 3;
  
  queryResult: any = {};
  makes: KeyValuePair[] = [];
  query: any = {
    pageSize: this.PAGE_SIZE,
  };
  columns: any[] =[
    { title: 'Id' },
    { title: 'Make', key: 'make', isSortable: true },
    { title: 'Model', key: 'model', isSortable: true },
    { title: 'Contact Name', key: 'contactName', isSortable: true }, 
    { title: 'View Details'}
  ];

  constructor(private vehicleService: VehicleService) { }

  ngOnInit() {
    this.vehicleService.getMakes().subscribe(makes => this.makes = makes);

    this.populateVehicles();  
    // console.log(typeof this.columns);
  }

  private populateVehicles() {
    this.vehicleService.filterVehicles(this.query)
      .subscribe(result => this.queryResult = result);
      
  }

  onFilterChange() {
    this.query.page = 1;
    //this.query.pageSize = this.PAGE_SIZE;
    this.populateVehicles();
    
  }

  resetFilter() {
    this.query = {
      page: 1,
      pageSize: this.PAGE_SIZE,
    };
    this.onFilterChange();
  }

  sortBy(columnName: any) {
    if (this.query.sortBy === columnName) {
      this.query.isSortAscending = !this.query.isSortAscending;
    } else {
      this.query.sortBy = columnName;
      this.query.isSortAscending = true;
    }
    this.populateVehicles();
  }

  onPageChange(page : any) {
    this.query.page = page;
    this.populateVehicles();
  }

}
