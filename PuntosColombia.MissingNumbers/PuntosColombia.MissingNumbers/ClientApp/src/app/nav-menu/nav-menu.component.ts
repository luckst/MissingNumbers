import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
    
  isLoggedIn$: Observable<boolean>;
  isExpanded = false;
  constructor(private authenticationService: AuthenticationService,
    private router: Router) { }

  ngOnInit(): void {
    this.isLoggedIn$ = this.authenticationService.isLoggedIn;
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  onLogout() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }
}
