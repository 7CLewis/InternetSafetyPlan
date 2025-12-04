export class CreateTeamCommand {
  public userEmail: string;
  public name: string;
  public description: string | null;

  constructor(userEmail:string, name: string, description: string) {
    this.userEmail = userEmail;
    this.name = name;
    this.description = description;
  }
}