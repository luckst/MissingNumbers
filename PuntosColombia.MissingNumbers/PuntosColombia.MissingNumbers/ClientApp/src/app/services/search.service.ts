import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Search } from '../models/search';

@Injectable()
export class SearchService {
  constructor(private http: HttpClient
    , @Inject('BASE_URL') private baseUrl: string) {

  }

  getSearches(userId: number, startDate: string, endDate: string) {
    return this.http.get(this.baseUrl + `api/search/GetSearches?userId=${userId}&startDate=${startDate}&endDate=${endDate}`).toPromise()
      .then(result => {
        return result;
      });
  }

  delete(id: number) {
    return this.http.get(this.baseUrl + `api/search/Delete?id=${id}`).toPromise()
      .then(result => {
        return result;
      });
  }

  doSearch(search: Search) {
    return this.http.post<any>(this.baseUrl + `api/search/DoSearch`,
      {
        QuantityListOne: search.quantityListOne,
        ListOne: search.listOne,
        QuantityListTwo: search.quantityListTwo,
        ListTwo: search.listTwo,
        UserId: search.userId
      }).toPromise()
      .then(result => {
        return result;
      });
  }
}
