export interface User {
  displayName: string;
  email: string;
  token: string;
  address: Address;
}

export interface Address {
  firstName: string;
  lastName: string;
  street: string;
  city: string;
  postalCode: string;
  country: string;
}
