import { Component, OnInit } from '@angular/core';
import { User } from '../models/user';
import { Search } from '../models/search';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SearchService } from '../services/search.service';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-missing-numbers',
  templateUrl: './missing-numbers.component.html',
  styleUrls: ['./missing-numbers.component.css']
})
/** missing-numbers component*/
export class MissingNumbersComponent implements OnInit {
  result: string = "";
  searchDetail: Search = null;
  currentUser: User;
  searches: Array<Search> = new Array<Search>();

  searchForm: FormGroup;
  filterForm: FormGroup;
  loading = false;
  submitted = false;
  message = "";
  showMessage = false;
  constructor(private formBuilder: FormBuilder,
    private router: Router,
    private searchService: SearchService,
    private authenticationService: AuthenticationService) {
    if (!this.authenticationService.getCurrentUser()) {
      this.router.navigate(['/login']);
    }
    this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
  }

  ngOnInit(): void {
    this.searchForm = this.formBuilder.group({
      quantityListOne: ['', Validators.required],
      listOne: ['', Validators.required],
      quantityListTwo: ['', Validators.required],
      listTwo: ['', Validators.required],

    });

    this.filterForm = this.formBuilder.group({
      startDate: [''],
      endDate: ['']

    });

    this.getSearches("", "");
  }

  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode === 32)
      return true;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;

  }

  get f() { return this.searchForm.controls; }
  get s() { return this.filterForm.controls; }

  getSearches(startDate: string, endDate: string) {
    this.searchService.getSearches(this.currentUser.userId, startDate, endDate)
      .then(
        data => {
          this.searches = null;
          if (data) {
            this.searches = data as Array<Search>;
            this.loading = false;
          }
        }).catch(error => {
          alert(error);
          this.loading = false;
        });
  }

  details(search: Search) {
    this.searchDetail = search;
  }

  hideDetails() {
    this.searchDetail = null;
  }

  delete(id: number) {
    if (confirm("¿Desea eliminar la busqueda?")) {
      this.searchService.delete(id)
        .then(
        data => {
          if (data["messageType"] === 1) {
              this.getSearches(this.s.startDate.value, this.s.endDate.value);
              alert("Busqueda eliminada");
            } else {
              this.loading = false;
              alert(data["message"]);
            }
          }).catch(error => {
            alert(error);
            this.loading = false;
          });
    }
  }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.searchForm.invalid) {
      return;
    }
    this.loading = true;
    let search = new Search();
    search.quantityListOne = this.f.quantityListOne.value;
    search.listOne = this.f.listOne.value;
    search.quantityListTwo = this.f.quantityListTwo.value;
    search.listTwo = this.f.listTwo.value;
    search.userId = this.currentUser.userId;

    this.searchService.doSearch(search)
      .then(
        data => {
          if (data.messageType === 1) {
            this.getSearches(this.s.startDate.value, this.s.endDate.value);
            this.loading = false;
            this.result = data.result;
          } else {
            this.loading = false;
            alert(data.message);
          }
        }).catch(error => {
          alert(error);
          this.loading = false;
        });
  }

  onSubmitFilter() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.filterForm.invalid) {
      return;
    }
    this.loading = true;

    this.getSearches(this.s.startDate.value, this.s.endDate.value)
  }
}
