import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';

import { CancellationService } from '../../enrollments/cancellation.service';
import { CancellationResponse } from '../../enrollments/cancellation.models';
import { PagedResult } from '../../../shared/models/paged-result.model';

@Component({
  selector: 'app-cancellation-history',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cancellation-history.html',
  styleUrl: './cancellation-history.css',
})
export class CancellationHistory implements OnInit {
  private readonly service = inject(CancellationService);

  requests = signal<CancellationResponse[]>([]);
  isLoading = signal(true);
  error = signal('');

  pageNumber = signal(1);
  pageSize = 10;
  totalRecords = signal(0);
  totalPages = signal(0);

  ngOnInit(): void {
    this.loadHistory();
  }

  loadHistory(): void {
    this.isLoading.set(true);
    this.error.set('');

    this.service
      .getMyCancellationRequests(this.pageNumber(), this.pageSize)
      .subscribe({
        next: (result: PagedResult<CancellationResponse>) => {
          this.requests.set(result.items);
          this.totalRecords.set(result.totalRecords);
          this.totalPages.set(result.totalPages);
          this.pageNumber.set(result.pageNumber);
          this.isLoading.set(false);
        },
        error: () => {
          this.error.set('Unable to load cancellation history.');
          this.isLoading.set(false);
        },
      });
  }

  previousPage(): void {
    if (this.pageNumber() > 1) {
      this.pageNumber.set(this.pageNumber() - 1);
      this.loadHistory();
    }
  }

  nextPage(): void {
    if (this.pageNumber() < this.totalPages()) {
      this.pageNumber.set(this.pageNumber() + 1);
      this.loadHistory();
    }
  }

  statusLabel(status: number): string {
    switch (status) {
      case 0:
        return 'Pending';
      case 1:
        return 'Approved';
      case 2:
        return 'Rejected';
      default:
        return 'Unknown';
    }
  }

  statusClass(status: number): string {
    switch (status) {
      case 0:
        return 'pending';
      case 1:
        return 'approved';
      case 2:
        return 'rejected';
      default:
        return 'unknown';
    }
  }

  formatDate(date: string | null): string {
    if (!date) return '—';

    return new Date(date).toLocaleString('en-IN', {
      dateStyle: 'medium',
      timeStyle: 'short',
    });
  }
}