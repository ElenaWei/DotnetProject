import { Router } from '@angular/router';
import { Injectable } from "@angular/core";
import * as auth0 from 'auth0-js';

(window as any).global = window;

@Injectable()
export class AuthService {

    auth0 = new auth0.WebAuth({
        clientID: 'E1n4bNmv5hoIR6bBOtu2GO5xIYPotEAX',
        domain: 'intelliswift-elena-dotnet.auth0.com',
        responseType: 'token id_token',
        audience: 'https://intelliswift-elena-dotnet.auth0.com/userinfo',
        redirectUri: 'http://localhost:5000/vehicles',
        scope: 'openid'
    });

    constructor(private router: Router) {}

    public login(): void {
        this.auth0.authorize();
    }
  // ...
  public handleAuthentication(): void {
    this.auth0.parseHash((err: any, authResult: any) => {
      if (authResult && authResult.accessToken && authResult.idToken) {
        window.location.hash = '';
        this.setSession(authResult);
        console.log(authResult);
        this.router.navigate(['/home']);
      } else if (err) {
        this.router.navigate(['/home']);
        console.log(err);
      }
    });
  }

  private setSession(authResult: any): void {
    // Set the time that the Access Token will expire at
    const expiresAt = JSON.stringify((authResult.expiresIn * 1000) + new Date().getTime());
    localStorage.setItem('access_token', authResult.accessToken);
    localStorage.setItem('id_token', authResult.idToken);
    localStorage.setItem('expires_at', expiresAt);
  }

  public logout(): void {
    // Remove tokens and expiry time from localStorage
    localStorage.removeItem('access_token');
    localStorage.removeItem('id_token');
    localStorage.removeItem('expires_at');
    // Go back to the home route
    this.router.navigate(['/']);
  }

  public isAuthenticated(): boolean {
    // Check whether the current time is past the
    // Access Token's expiry time
    const expiresAt = JSON.parse(localStorage.getItem('expires_at') || '{}');
    return new Date().getTime() < expiresAt;
  }

}
