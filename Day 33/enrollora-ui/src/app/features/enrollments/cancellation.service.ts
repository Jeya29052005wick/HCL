import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { PagedResult } from '../../shared/models/paged-result.model';

import {
  CancellationResponse,
  CancellationFilter,
  ReviewCancellationRequest,
} from './cancellation.models';

@Injectable({
  providedIn: 'root',
})
export class CancellationService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/cancellation-requests`;

  getCancellationRequests(
    pageNumber = 1,
    pageSize = 10,
    filters?: CancellationFilter,
  ): Observable<PagedResult<CancellationResponse>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize)
      .set('sortOrder', 'desc');

    if (filters?.search) {
      params = params.set('search', filters.search);
    }

    if (filters?.status !== null && filters?.status !== undefined) {
      params = params.set('status', filters.status);
    }

    return this.http.get<PagedResult<CancellationResponse>>(this.apiUrl, {
      params,
    });
  }

  approveRequest(
    id: number,
    request: ReviewCancellationRequest,
  ): Observable<CancellationResponse> {
    return this.http.put<CancellationResponse>(
      `${this.apiUrl}/${id}/approve`,
      request,
    );
  }

  rejectRequest(
    id: number,
    request: ReviewCancellationRequest,
  ): Observable<CancellationResponse> {
    return this.http.put<CancellationResponse>(
      `${this.apiUrl}/${id}/reject`,
      request,
    );
  }
}