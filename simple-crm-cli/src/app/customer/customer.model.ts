// Allows custom status codes to be defined

export type InteractionMethod = 'phone' | 'email';

export interface Customer {
  customerId: number;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  emailAddress: string;
  preferredContactMethod: InteractionMethod;
  statusCode: string;
  lastContactDate: string; // ISO format date
}

export interface CustomerSaveResult {
  success: boolean;
  messages: string[];
  item: Customer;
}
