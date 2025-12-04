export class DeleteTeammateCommand {
  public teamId: string;
  public teammateId: string;

  constructor(teamId: string, teammateId: string) {
    this.teamId = teamId;
    this.teammateId = teammateId;
  }
}