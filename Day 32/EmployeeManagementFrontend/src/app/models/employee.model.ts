
export interface Employee {
  id: number;
  name: string;
  email: string;
  phone: string;
  address: string | null;
  departmentId: number;
  departmentName: string;
  joiningDate: string;
}

export interface CreateEmployee {
  name: string;
  email: string;
  phone: string;
  address: string | null;
  departmentId: number;
  joiningDate: string;
}

export interface UpdateEmployee {
  name: string;
  email: string;
  phone: string;
  address: string | null;
  departmentId: number;
  joiningDate: string;
}
