export interface Movie { movieId:number; title:string; description?:string|null; durationMinutes:number; releaseDate:string; language:string; genre:string; }
export interface Theatre { theatreId:number; name:string; location:string; }
export interface Screen { screenId:number; theatreId:number; screenName:string; }
export interface Seat { seatId:number; screenId:number; rowName:string; seatNumber:number; seatType:string; }
export interface Show { showId:number; movieId:number; movieTitle:string; screenId:number; screenName:string; theatreId:number; theatreName:string; startTime:string; endTime:string; ticketPrice:number; }
export interface ShowSeat { seatId:number; rowName:string; seatNumber:number; seatLabel:string; seatType:string; price:number; status:string; }
export interface Booking { bookingId:number; bookingReference:string; showId:number; customerName:string; customerEmail:string; customerPhone:string; bookingDate:string; totalAmount:number; status:string; seats:string[]; }
export interface LoginResponse { message:string; token:string; }
export interface ApiError { message?:string; title?:string; errors?:Record<string,string[]>; }
