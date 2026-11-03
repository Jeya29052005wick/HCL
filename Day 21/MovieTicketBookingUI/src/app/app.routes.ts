import { Routes } from '@angular/router';
import { HomePage } from './pages/home.page';
import { MoviesPage } from './pages/movies.page';
import { MovieDetailPage } from './pages/movie-detail.page';
import { BookingPage } from './pages/booking.page';
import { AuthPage } from './pages/auth.page';
import { AdminPage } from './pages/admin.page';
import { roleGuard } from './core/role.guard';
export const routes:Routes=[
 {path:'',component:HomePage},{path:'movies',component:MoviesPage},{path:'movies/:id',component:MovieDetailPage},{path:'book/:showId',component:BookingPage},{path:'login',component:AuthPage,data:{mode:'login'}},{path:'register',component:AuthPage,data:{mode:'register'}},{path:'manage',component:AdminPage,canActivate:[roleGuard],data:{roles:['Admin','TheatreManager']}},{path:'**',redirectTo:''}
];
