import { Teammate } from 'library/teamAggregate/Teammate';

export class Team {
  public id: string;
  public name: string;
  public teammates: Teammate[];

  constructor(id: string, name: string, teammates: Teammate[]) {
    this.id = id;
    this.name = name;
    this.teammates = teammates;
  }
}