import { Email } from 'library/shared/Email';

export class User {
  public id: string;
  public email: Email;

  constructor(id: string, email: string) {
    this.id = id;
    this.email = new Email(email);
  }
}