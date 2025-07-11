public class TakingTurnsQueue
{
    private readonly PersonQueue _people = new();

    public int Length => _people.Length;

    public void AddPerson(string name, int turns)
    {
        var person = new Person(name, turns);
        _people.Enqueue(person);
    }

    public Person GetNextPerson()
    {
        if (_people.IsEmpty())
        {
            throw new InvalidOperationException("No one in the queue.");
        }

        Person person = _people.Dequeue();

        if (person.Turns <= 0) // Infinite turns
        {
            _people.Enqueue(person);
        }
        else if (person.Turns > 1) // Finite turns remaining
        {
            person.Turns--;
            _people.Enqueue(person);
        }
        // If turns == 1, don't requeue

        return person;
    }

    public override string ToString()
    {
        return _people.ToString();
    }
}