export class UpdateTeammateInformationCommand {
  public teamId: string;
  public teammateId: string;
  public name: string;
  public dateOfBirth: string | null;
  public userId: string | null;

  constructor(teamId: string, teammateId: string, name: string, dateOfBirth: string | null, userId: string | null) {
    this.teamId = teamId;
    this.teammateId = teammateId;
    this.name = name;
    this.dateOfBirth = dateOfBirth;
    this.userId = userId;
  }
}