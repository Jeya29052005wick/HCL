export interface Department {
  id: number;
  name: string;
  description: string | null;
}

export interface CreateDepartment {
  name: string;
  description: string | null;
}

export interface UpdateDepartment {
  name: string;
  description: string | null;
}