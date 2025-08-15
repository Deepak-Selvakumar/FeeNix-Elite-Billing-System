import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  hidePassword = true;

  constructor(
    private fb: FormBuilder,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      userId: ['', Validators.required],
      password: ['', Validators.required],
      recaptcha: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    // You might want to add remember me functionality here
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      // Here you would typically call your authentication service
      console.log('Login form submitted', this.loginForm.value);
      // Simulate successful login
      // this.router.navigate(['/dashboard']);
    }
  }

  onCaptchaResolved(captchaResponse: string): void {
    console.log(`Resolved captcha with response: ${captchaResponse}`);
  }

  navigateToCreateAccount(): void {
    this.router.navigate(['/create-account']);
  }
}