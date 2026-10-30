import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface AppLocation {
  LocationId: number;
  Location_Code: string;
  Location_Name: string;
}

@Injectable({
  providedIn: 'root'
})
export class Locations {

  private apiUrl = 'https://localhost:7029/api/Locations';

  constructor(private http: HttpClient) { }

  getLocations(): Observable<AppLocation[]> {
    return this.http.get<AppLocation[]>(this.apiUrl);
  }
}