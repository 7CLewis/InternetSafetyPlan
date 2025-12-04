export class AddTeammateToTeamCommand {
  public teamId: string;
  public name: string;
  public dateOfBirth: string | null;

  constructor(teamId: string, name: string, dateOfBirth: string) {
    this.teamId = teamId;
    this.name = name;
    this.dateOfBirth = dateOfBirth;
  }
}