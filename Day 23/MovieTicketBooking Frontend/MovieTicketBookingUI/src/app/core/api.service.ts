import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Movie, Theatre, Screen, Seat, Show, ShowSeat, Booking, LoginResponse } from './models';
@Injectable({providedIn:'root'})
export class ApiService {
  private http=inject(HttpClient); private base=environment.apiBaseUrl;
  private url(path:string){return `${this.base}/${path}`;}
  movies(pageNumber=1,pageSize=50,title?:string):Observable<Movie[]>{let p=new HttpParams().set('pageNumber',pageNumber).set('pageSize',pageSize);if(title)p=p.set('title',title);return this.http.get<Movie[]>(this.url('movies'),{params:p});}
  movie(id:number){return this.http.get<Movie>(this.url(`movies/${id}`));}
  createMovie(body:Partial<Movie>){return this.http.post<Movie>(this.url('movies'),body);}
  updateMovie(id:number,body:Partial<Movie>){return this.http.put<Movie>(this.url(`movies/${id}`),body);}
  deleteMovie(id:number){return this.http.delete<void>(this.url(`movies/${id}`));}
  theatres(pageNumber=1,pageSize=50){return this.http.get<Theatre[]>(this.url('theatres'),{params:{pageNumber,pageSize}});}
  theatre(id:number){return this.http.get<Theatre>(this.url(`theatres/${id}`));}
  createTheatre(body:Partial<Theatre>){return this.http.post<Theatre>(this.url('theatres'),body);}
  updateTheatre(id:number,body:Partial<Theatre>){return this.http.put<Theatre>(this.url(`theatres/${id}`),body);}
  deleteTheatre(id:number){return this.http.delete<void>(this.url(`theatres/${id}`));}
  screens(theatreId?:number){let p=new HttpParams();if(theatreId)p=p.set('theatreId',theatreId);return this.http.get<Screen[]>(this.url('screens'),{params:p});}
  createScreen(body:Partial<Screen>){return this.http.post<Screen>(this.url('screens'),body);}
  updateScreen(id:number,body:Partial<Screen>){return this.http.put<Screen>(this.url(`screens/${id}`),body);}
  deleteScreen(id:number){return this.http.delete<void>(this.url(`screens/${id}`));}
  seats(screenId:number){return this.http.get<Seat[]>(this.url(`seats/screen/${screenId}`));}
  bulkSeats(body:{screenId:number;seats:{rowName:string;seatNumber:number;seatType:string}[]}){return this.http.post<Seat[]>(this.url('seats/bulk'),body);}
  show(id:number){return this.http.get<Show>(this.url(`shows/${id}`));}
  shows(filters:{date?:string;movieId?:number;theatreId?:number}={}){let p=new HttpParams();Object.entries(filters).forEach(([k,v])=>{if(v!==undefined&&v!==null&&v!=='')p=p.set(k,String(v));});return this.http.get<Show[]>(this.url('shows'),{params:p});}
  createShow(body:Partial<Show>){return this.http.post<Show>(this.url('shows'),body);}
  updateShow(id:number,body:Partial<Show>){return this.http.put<Show>(this.url(`shows/${id}`),body);}
  deleteShow(id:number){return this.http.delete<void>(this.url(`shows/${id}`));}
  showSeats(showId:number){return this.http.get<ShowSeat[]>(this.url(`shows/${showId}/seats`));}
  login(email:string,password:string){return this.http.post<LoginResponse>(this.url('auth/login'),{email,password});}
  register(fullName:string,email:string,password:string){return this.http.post<{message:string}>(this.url('auth/register'),{fullName,email,password});}
  createBooking(body:{customerName:string;customerEmail:string;customerPhone:string;showId:number;seatIds:number[]}){return this.http.post<Booking>(this.url('bookings'),body);}
  booking(id:number){return this.http.get<Booking>(this.url(`bookings/${id}`));}
  cancelBooking(id:number){return this.http.put<Booking>(this.url(`bookings/${id}/cancel`),{});}
}
