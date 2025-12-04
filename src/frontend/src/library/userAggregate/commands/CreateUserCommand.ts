export class CreateUserCommand {
  public email: string;

  constructor(email:string) {
    this.email = email;
  }
}