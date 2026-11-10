
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ApiService } from '../core/api.service';
import { Movie } from '../core/models';

@Component({
  selector: 'app-movies-page',
  standalone: true,
  imports: [FormsModule, RouterLink],
  template: `
    <section class="page-wrap movies-catalogue">

      <div class="page-heading">
        <div>
          <div class="eyebrow">THE SECRET COLLECTION</div>

          <h1>
            Discover your next<br>
            <em>favourite story.</em>
          </h1>

          <p>Explore our collection of movies and find your next cinema experience.</p>
        </div>

        <div class="catalogue-count">
          <b>{{ filtered.length.toString().padStart(2, '0') }}</b>
          <span>MOVIES<br>AVAILABLE</span>
        </div>
      </div>

      <!-- SEARCH AND FILTER -->
      <div class="catalogue-tools">

        <label class="search-field">
          <span>⌕</span>

          <input
            type="text"
            [(ngModel)]="query"
            placeholder="Search movies by title, genre or language">
        </label>

        <select [(ngModel)]="genre">
          <option value="">All Genres</option>

          @for (g of genres; track g) {
            <option [value]="g">{{ g }}</option>
          }
        </select>

      </div>

      <!-- LOADING -->
      @if (loading) {
        <div class="loading-line">
          Loading movies...
        </div>
      }

      <!-- ERROR -->
      @else if (error) {
        <div class="notice">
          {{ error }}

          <button class="retry-button" (click)="loadMovies()">
            Try Again
          </button>
        </div>
      }

      <!-- MOVIE GRID -->
      @else if (filtered.length > 0) {

        <div class="catalogue-grid">

          @for (movie of filtered; track movie.movieId; let i = $index) {

            <article class="catalogue-card">

              <div
                class="catalogue-poster movie-text-art"
                [attr.data-index]="i % 5">

                <span class="movie-art-label">SECRET TOMATOES</span>

                <div class="movie-art-title">
                  {{ movie.title }}
                </div>

                <span class="movie-art-id">
                  FILM NO. {{ movie.movieId.toString().padStart(3, '0') }}
                </span>

                <span class="catalogue-open">↗</span>

              </div>

              <div class="catalogue-info">

                <div>
                  <h3>{{ movie.title }}</h3>

                  <p>
                    {{ movie.genre }}
                    <i>·</i>
                    {{ movie.language }}
                  </p>
                </div>

                <span>{{ movie.durationMinutes }}m</span>

              </div>

              <a
                class="movie-view-link"
                [routerLink]="['/movies', movie.movieId]">
                VIEW MOVIE DETAILS →
              </a>

            </article>

          }

        </div>

      }

      <!-- EMPTY -->
      @else {
        <div class="empty-panel">
          <h3>No movies found.</h3>
          <p>Try another movie title or select a different genre.</p>

          <button class="retry-button" (click)="clearFilters()">
            Clear Filters
          </button>
        </div>
      }

    </section>
  `
})
export class MoviesPage implements OnInit {

  private api = inject(ApiService);

  movies: Movie[] = [];

  query = '';
  genre = '';

  loading = true;
  error = '';

  ngOnInit(): void {
    this.loadMovies();
  }

  loadMovies(): void {

    this.loading = true;
    this.error = '';

    this.api.movies(1, 100).subscribe({

      next: (response) => {
        this.movies = response;
        this.loading = false;
      },

      error: (err) => {
        console.error('Movie loading error:', err);

        this.error = 'Unable to load movies. Please check your backend connection.';
        this.loading = false;
      }

    });
  }

  get genres(): string[] {
    return [
      ...new Set(
        this.movies
          .map(movie => movie.genre)
          .filter(genre => !!genre)
      )
    ].sort();
  }

  get filtered(): Movie[] {

    const search = this.query.trim().toLowerCase();

    return this.movies.filter(movie => {

      const matchesSearch =
        !search ||
        `${movie.title} ${movie.genre} ${movie.language}`
          .toLowerCase()
          .includes(search);

      const matchesGenre =
        !this.genre || movie.genre === this.genre;

      return matchesSearch && matchesGenre;

    });
  }

  clearFilters(): void {
    this.query = '';
    this.genre = '';
  }
}
