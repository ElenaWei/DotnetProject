import { ToastyService } from 'ng2-toasty';
import { PhotoService } from './../../services/photo.service';
import { Router, ActivatedRoute } from '@angular/router';
import { VehicleService } from './../../services/vehicle.service';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-view-vehicle',
  templateUrl: './view-vehicle.component.html',
  styleUrls: ['./view-vehicle.component.css']
})
export class ViewVehicleComponent implements OnInit {

  @ViewChild('fileInput')fileInput: any = ElementRef;

  vehicle : any;
  vehicleId : number = 0;
  photos: any[] = [];
  progress: any;

  constructor(
    private vehicleService : VehicleService,
    private router : Router,
    private route : ActivatedRoute,
    private photoService : PhotoService,
    private toasty : ToastyService) { 
      route.params.subscribe ( p => {
        this.vehicleId = +p['id'];
        // validation check
        if (isNaN(this.vehicleId) || this.vehicleId <= 0) {
          router.navigate(['/vehicles']);
          return;
        }
      });
    }

  ngOnInit() {
    // get photos for the current vehicle
    this.photoService.getPhotos(this.vehicleId)
    .subscribe(p => this.photos = p);

    // get vehicle object by vehicleId
    this.vehicleService.getVehicle(this.vehicleId)
    .subscribe(
      v => this.vehicle = v,
      err => {
        if (err.status == 404) {
          this.router.navigate(['/vehicles']);
          return;
        }
      });

  }
  uploadPhoto() {
    var nativeElement: HTMLInputElement = this.fileInput.nativeElement;
  
    var file = nativeElement.files ? nativeElement.files[0] : null;
    
    this.photoService.upload(this.vehicleId, file)
        .subscribe(photo => {console.log(photo);
          this.photos.push(photo);},
          err => {
            this.toasty.error({
              title: 'Error',
              msg: err.text(),
              theme: 'bootstrap',
              showClose: true,
              timeout: 5000
            })
          });
    
  }

  delete() {
    if(confirm("Are you sure?")) {
      this.vehicleService.delete(this.vehicle.id)
      .subscribe(x => {
        this.router.navigate(['/vehicles']);
      });
    }
  }

}
