export interface User {
  id: number;
  username: string;
  firstName: string;
  lastName: string;
  password: string;
  type: any;
  isActive: boolean;
  token?: string;
}
