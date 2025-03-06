using UnityEngine;
using TMPro;
using UnityEngine.UI;

/*
     * HoverManager.cs
     * Created by: Nicolas Kaplan
     * Date Created: 2025-03-08
     * 
     * Description: This code manages the hint / card system that allows players to
     *              right-click most objects to display more information about them.
     *              This works with power-ups, UI elements, and game elements too.
     * 
     * Last Changed by: Nicolas Kaplan
     * Last Date Changed: 2025-03-08
     * 
     * 
     *   -> 1.0 - Created HoverManager.cs and created baseline code to be used on
     *          hoverable objects that will provide info when right-clicked.
     *   v1.0
     */
public class HoverManager : MonoBehaviour
{
    // TODO animations for the card???
    public static HoverManager Instance;

    public GameObject infoCardUI;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public RectTransform cardRectTransform;

    private HoverableObject currentHoverObject;
    private bool infoCardVisible = false;
    
    private Vector2 cursorOffset = new Vector2(20f, -10f); 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        DetectHover();
        HandleRightClick();
        MoveCardToCursor();
    }

    void DetectHover()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero); 

        if (hit.collider != null)
        {
            HoverableObject hoverable = hit.collider.GetComponent<HoverableObject>();
            if (hoverable != null)
            {
                currentHoverObject = hoverable;
                return;
            }
        }

        currentHoverObject = null;
    }

    void HandleRightClick()
    {
        if (Input.GetMouseButtonDown(1) && currentHoverObject != null)
        {
            if (!infoCardVisible) ShowInfoCard();
            else HideInfoCard();
        }
    }

    public void ShowInfoCard()
    {
        infoCardVisible = true;
        infoCardUI.SetActive(true);
        nameText.text = currentHoverObject.DisplayName;
        descriptionText.text = currentHoverObject.Description;

        LayoutRebuilder.ForceRebuildLayoutImmediate(cardRectTransform); 
    }

    public void HideInfoCard()
    {
        infoCardVisible = false;
        infoCardUI.SetActive(false);
    }

    private void MoveCardToCursor()
{
    if (infoCardVisible)
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 anchoredPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            cardRectTransform.parent as RectTransform, 
            mousePosition, 
            Camera.main, 
            out anchoredPosition
        );

        anchoredPosition += cursorOffset;

        float cardWidth = cardRectTransform.rect.width;
        float cardHeight = cardRectTransform.rect.height;
        float maxX = (Screen.width / 2) - cardWidth;
        float maxY = (Screen.height / 2) - cardHeight;

        anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, -maxX, maxX);
        anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, -maxY, maxY);

        cardRectTransform.anchoredPosition = anchoredPosition;
    }
}

}
