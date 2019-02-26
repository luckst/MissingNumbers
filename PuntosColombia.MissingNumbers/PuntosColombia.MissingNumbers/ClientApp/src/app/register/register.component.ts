import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { User } from '../models/user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
/** register component*/
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;
  loading = false;
  submitted = false;
  returnUrl: string;
  showMessage = false;

  constructor(private formBuilder: FormBuilder,
    private router: Router,
    private authenticationService: AuthenticationService) {
    if (this.authenticationService.getCurrentUser()) {
      this.router.navigate(['/']);
    }
  }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      documentType: ['', Validators.required],
      documentNumber: ['', Validators.required],
      email: ['', Validators.required],

    });
  }

  get f() { return this.registerForm.controls; }

  onSubmit() {


    this.submitted = true;

    // stop here if form is invalid
    if (this.registerForm.invalid) {
      return;
    }
    debugger;
    this.loading = true;
    let user = new User();
    user.documentNumber = this.f.documentNumber.value;
    user.documentType = this.f.documentType.value;
    user.email = this.f.email.value;
    user.userName = this.f.username.value;
    user.password = this.f.password.value;

    this.authenticationService.register(user)
      .then(
        data => {
          debugger;
          if (data.model.messageType === 1) {
            this.router.navigate(['/login']);
          } else {
            alert(data.model.message);
            this.loading = false;
          }
        }).catch(error => {
          debugger;
          alert(error);
          this.loading = false;
        });
  }
}
