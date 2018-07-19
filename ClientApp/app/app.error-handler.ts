import { ToastyService } from 'ng2-toasty';
import { ErrorHandler, NgZone, Inject } from "@angular/core";


export class AppErroHandler implements ErrorHandler {

    constructor(
        private ngZone: NgZone,
        @Inject(ToastyService) private toastyService: ToastyService
    ) {}

    handleError(error: any) : void {
        this.ngZone.run(() => {
            this.toastyService.error({
                title: 'Error',
                msg: 'An unexpected error happened',
                theme: 'bootstrap',
                showClose: true,
                timeout: 5000
            });
        });
    }
}