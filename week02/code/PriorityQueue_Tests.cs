using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Requirement: Higher priority items should dequeue first
    // Scenario: Queue with Low(1), High(3), Medium(2)
    // Expected: High, Medium, Low
    // Result: PASS after fixing priority comparison
    public void TestPriorityQueue_BasicPriority()
    {
        var queue = new PriorityQueue();
        queue.Enqueue("Low", 1);
        queue.Enqueue("High", 3);
        queue.Enqueue("Medium", 2);

        Assert.AreEqual("High", queue.Dequeue());
        Assert.AreEqual("Medium", queue.Dequeue());
        Assert.AreEqual("Low", queue.Dequeue());
    }

    [TestMethod]
    // Requirement: Same priority items should follow FIFO order
    // Scenario: Queue with First(1), Second(1), Third(1)
    // Expected: First, Second, Third
    // Result: PASS after changing to > comparison
    public void TestPriorityQueue_SamePriorityFIFO()
    {
        var queue = new PriorityQueue();
        queue.Enqueue("First", 1);
        queue.Enqueue("Second", 1);
        queue.Enqueue("Third", 1);

        Assert.AreEqual("First", queue.Dequeue());
        Assert.AreEqual("Second", queue.Dequeue());
        Assert.AreEqual("Third", queue.Dequeue());
    }

    [TestMethod]
    // Requirement: Empty queue should throw exception
    // Scenario: Dequeue from empty queue
    // Expected: InvalidOperationException
    // Result: PASS (original implementation was correct)
    public void TestPriorityQueue_EmptyQueue()
    {
        var queue = new PriorityQueue();
        Assert.ThrowsException<InvalidOperationException>(() => queue.Dequeue());
    }

    [TestMethod]
    // Requirement: Complex priority mixing
    // Scenario: Queue with mixed priorities including duplicates
    // Expected: D(3), B(2), C(2), A(1), E(1)
    // Result: PASS after all fixes implemented
    public void TestPriorityQueue_MixedPriorities()
    {
        var queue = new PriorityQueue();
        queue.Enqueue("A", 1);
        queue.Enqueue("B", 2);
        queue.Enqueue("C", 2);
        queue.Enqueue("D", 3);
        queue.Enqueue("E", 1);

        Assert.AreEqual("D", queue.Dequeue());
        Assert.AreEqual("B", queue.Dequeue());
        Assert.AreEqual("C", queue.Dequeue());
        Assert.AreEqual("A", queue.Dequeue());
        Assert.AreEqual("E", queue.Dequeue());
    }
}