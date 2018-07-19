import { Injectable } from '@angular/core';
import { Subject } from "rxjs/Subject";
import { BrowserXhr } from '@angular/http';

/* 
 use XMLHttpRequest object in the browser to create the progress of upload and download
 then push this progress to the obserable need to be returned 
*/
@Injectable()
export class ProgressService {

    uploadProgress: Subject<any> = new Subject();
    //downloadProgress: Subject<any> = new Subject();
    startTracking() {
        this.uploadProgress = new Subject();
        return this.uploadProgress;
    }

    notify(progress: any) {
        this.uploadProgress.next(progress);
    }

    endTracking() {
        this.uploadProgress.complete();
    }

}

// implement custom Browser Xhr extends from angular class BrowserXhr
@Injectable()
export class BrowserXhrWithProgress extends BrowserXhr {
    constructor(private service: ProgressService) { super(); }

    build(): XMLHttpRequest {
        var xhr: XMLHttpRequest = super.build();

        // xhr.onprogress = (event) => {
        //     this.service.downloadProgress.next(this.createProgress(event));
        // }
        xhr.upload.onprogress = (event) => {
            this.service.notify(this.createProgress(event));
        }

        xhr.upload.onloadend = () => {
            this.service.endTracking();
        }
        return xhr;       
    }

    private createProgress(event : any) {
        return {
            total:event.total,
            percentage: Math.round(event.loaded / event.total * 100)
        };
    }
}