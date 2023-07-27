namespace TungaRestaurant.Models
{
    public enum UserStatus
    {
        NORMAL,
        SUSPEND,
        BAN
    }
    public enum FoodStatus
    {
        AVAILABLE,
        UNAVAILABLE
    }
    public enum ReservationStatus
    {
        USING,
        END
    }

   
    public enum TableStatus
    {
        EMPTY,
        BOOKED,
        USING
    }
    public enum Sex
    {
        FEMALE,
        MALE,
        OTHER
    }

    public enum Role
    {
        ADMIN,
        BRANCH_MANAGER,
        CUSTOMER
    }

    public enum OrderStatus
    {
        QUEUED,
        COOKING,
        DELIVERING,
        DELIVERED,
        CANCELED
    }
    public enum BranchStatus
    {
        OPEN,
        CLOSE
    }
}
