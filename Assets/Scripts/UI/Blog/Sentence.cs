using UnityEngine;

public class Sentence : MonoBehaviour
{
    [SerializeField]
    private string beforeAns;
    [SerializeField]
    public string answer;
    [SerializeField]
    public string afterAns;
    public Sentence(string part1, string answer, string part2)
    {
        this.beforeAns = part1;
        this.answer = answer;
        this.afterAns = part2;
    }
    public Sentence(string part1, string answer)
    {
        this.beforeAns = part1;
        this.answer = answer;
    }
    public string GetSentence()
    {
        return beforeAns + answer + afterAns;
    }
    public string GetAnswer()
    {
        return answer;
    }
    public bool CheckAnswer(string given)
    {
        string givenAns = given.ToLower().Trim();
        return givenAns==answer;
    }
}
