import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CancellationRequestService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/enrollments`;

  requestCancellation(
    enrollmentId: number,
    reason: string,
  ): Observable<unknown> {
    return this.http.post(
      `${this.apiUrl}/${enrollmentId}/cancellation-request`,
      { reason },
    );
  }
}