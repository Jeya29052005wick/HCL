import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { CancellationService } from '../../enrollments/cancellation.service';

import {
  CancellationResponse,
  CancellationStatus,
} from '../../enrollments/cancellation.models';

@Component({
  selector: 'app-cancellation-requests',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './cancellation-requests.html',
  styleUrl: './cancellation-requests.css',
})
export class CancellationRequests implements OnInit {
  private cancellationService = inject(CancellationService);

  requests = signal<CancellationResponse[]>([]);

  isLoading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  pageNumber = signal(1);
  pageSize = signal(10);
  totalRecords = signal(0);
  totalPages = signal(0);

  search = '';
  selectedStatus: number | null = null;

  selectedRequest = signal<CancellationResponse | null>(null);

  reviewRemarks = '';
  reviewAction: 'approve' | 'reject' | null = null;
  isReviewing = signal(false);

  readonly pendingCount = computed(
    () => this.requests().filter((x) => x.status === CancellationStatus.Pending).length,
  );

  readonly approvedCount = computed(
    () => this.requests().filter((x) => x.status === CancellationStatus.Approved).length,
  );

  readonly rejectedCount = computed(
    () => this.requests().filter((x) => x.status === CancellationStatus.Rejected).length,
  );

  ngOnInit(): void {
    this.loadRequests();
  }

  loadRequests(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.cancellationService
      .getCancellationRequests(this.pageNumber(), this.pageSize(), {
        search: this.search.trim(),
        status: this.selectedStatus,
      })
      .subscribe({
        next: (response) => {
          this.requests.set(response.items);
          this.totalRecords.set(response.totalRecords);
          this.totalPages.set(response.totalPages);
          this.pageNumber.set(response.pageNumber);
          this.pageSize.set(response.pageSize);

          this.isLoading.set(false);
        },
        error: (error) => {
          console.error('Failed to load cancellation requests:', error);

          this.errorMessage.set(
            error?.error?.message || 'Unable to load cancellation requests.',
          );

          this.isLoading.set(false);
        },
      });
  }

  applyFilters(): void {
    this.pageNumber.set(1);
    this.loadRequests();
  }

  clearFilters(): void {
    this.search = '';
    this.selectedStatus = null;
    this.pageNumber.set(1);

    this.loadRequests();
  }

  nextPage(): void {
    if (this.pageNumber() < this.totalPages()) {
      this.pageNumber.update((page) => page + 1);
      this.loadRequests();
    }
  }

  previousPage(): void {
    if (this.pageNumber() > 1) {
      this.pageNumber.update((page) => page - 1);
      this.loadRequests();
    }
  }

  viewRequest(request: CancellationResponse): void {
    this.selectedRequest.set(request);
  }

  closeDetails(): void {
    this.selectedRequest.set(null);
  }

  openReview(
    request: CancellationResponse,
    action: 'approve' | 'reject',
  ): void {
    this.selectedRequest.set(request);
    this.reviewAction = action;
    this.reviewRemarks = '';
  }

  closeReview(): void {
    this.reviewAction = null;
    this.reviewRemarks = '';
    this.selectedRequest.set(null);
  }

  submitReview(): void {
    const request = this.selectedRequest();

    if (!request || !this.reviewAction || this.isReviewing()) {
      return;
    }

    const action = this.reviewAction;

    const confirmed = confirm(
      action === 'approve'
        ? 'Are you sure you want to approve this cancellation request?'
        : 'Are you sure you want to reject this cancellation request?',
    );

    if (!confirmed) {
      return;
    }

    this.isReviewing.set(true);
    this.errorMessage.set('');

    const reviewData = {
      adminRemarks: this.reviewRemarks.trim() || null,
    };

    const operation =
      action === 'approve'
        ? this.cancellationService.approveRequest(
            request.cancellationRequestId,
            reviewData,
          )
        : this.cancellationService.rejectRequest(
            request.cancellationRequestId,
            reviewData,
          );

    operation.subscribe({
      next: () => {
        this.isReviewing.set(false);
        this.reviewAction = null;
        this.reviewRemarks = '';
        this.selectedRequest.set(null);

        this.successMessage.set(
          action === 'approve'
            ? 'Cancellation request approved successfully.'
            : 'Cancellation request rejected successfully.',
        );

        this.loadRequests();
      },
      error: (error) => {
        console.error('Review failed:', error);

        this.isReviewing.set(false);

        this.errorMessage.set(
          error?.error?.message || 'Unable to review cancellation request.',
        );
      },
    });
  }

  getStatusLabel(status: number): string {
    switch (status) {
      case CancellationStatus.Pending:
        return 'Pending';
      case CancellationStatus.Approved:
        return 'Approved';
      case CancellationStatus.Rejected:
        return 'Rejected';
      default:
        return 'Unknown';
    }
  }
}