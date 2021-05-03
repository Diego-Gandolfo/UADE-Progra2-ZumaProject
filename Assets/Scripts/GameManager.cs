using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]private List<Ball> ballList;
	public QueueDymamic queueDynamic;
	public Node<Ball> rootNode;
	//Node<Ball> auxNode;
	//Node<Ball> auxNodeSupp = new Node<Ball>();
	private void Start()
	{
		
	}
	public void CheckColors(Ball ball)
	{

		print("Check Color");
		rootNode = queueDynamic.rootNode;

		var auxNode = rootNode;
		ballList = new List<Ball>();
		ballList.Add(ball);
		while (auxNode.element != ball && auxNode.nextNode != null)
		{
			auxNode = auxNode.nextNode;
		}

		if (auxNode.element == ball)
		{		
			if (auxNode.nextNode != null)
			{
				var auxNodeSupp = auxNode.nextNode;
				while (auxNode.element.gameObject.GetComponent<SpriteRenderer>().color == auxNodeSupp.element.gameObject.GetComponent<SpriteRenderer>().color && auxNodeSupp.nextNode != null)
				{
					ballList.Add(auxNodeSupp.element);
					auxNodeSupp = auxNodeSupp.nextNode;
					
				}
				ballList.Add(auxNodeSupp.element);
			}
			if(auxNode.previousNode != null)
            {
				var auxNodeSupp = auxNode.previousNode;
				while (auxNode.element.gameObject.GetComponent<SpriteRenderer>().color == auxNodeSupp.element.gameObject.GetComponent<SpriteRenderer>().color && auxNodeSupp.previousNode != null)
				{
					ballList.Add(auxNodeSupp.element);
					auxNodeSupp = auxNodeSupp.previousNode;
				}
				ballList.Add(auxNodeSupp.element);
			}



		}
        if (ballList.Count >= 3)
        {
            for (int i = 0; i < ballList.Count - 1; i++)
            {
				
				queueDynamic.DesqueueMiddle(ballList[i]);
            }
        }

	}
}
