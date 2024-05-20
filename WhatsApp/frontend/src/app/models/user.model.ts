class User {
  constructor(
    public _id: string,
    public name: string,
    public email: string,
    public phone: string,
    public contactList: User[],
    public isOnline: boolean,
    public image: string,
    public lastSeen: Date
  ) {}
}
export default User;
