import { PagedResult } from '../../shared/models/paged-result.model';

export enum CancellationStatus {
  Pending = 0,
  Approved = 1,
  Rejected = 2,
}

export interface CancellationResponse {
  cancellationRequestId: number;
  enrollmentId: number;
  studentId: number;
  studentName: string;
  studentEmail: string;
  courseName: string;
  reason: string;
  status: CancellationStatus;
  requestedAt: string;
  reviewedAt: string | null;
  adminRemarks: string | null;
}

export interface ReviewCancellationRequest {
  adminRemarks?: string | null;
}

export interface CancellationFilter {
  search?: string;
  status?: CancellationStatus | null;
}

export type CancellationPagedResult = PagedResult<CancellationResponse>;