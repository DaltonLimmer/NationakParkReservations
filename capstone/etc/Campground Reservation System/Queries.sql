--Select campground.name, site.site_id, reservation.name, reservation.from_date, reservation.to_date
--From campground 
--JOIN site ON site.campground_id = campground.campground_id
--JOIN reservation ON reservation.site_id = site.site_id

--SELECT * FROM campground JOIN park ON park.park_id = campground.park_id WHERE  park.name = @parkName;

--SELECT * From campground JOIN park ON park.park_id = campground.park_id

--SELECT site.site_id reservation.* FROM campground
--JOIN site ON site.campground_id = campground.campground_id
--JOIN reservation ON reservation.site_id = site.site_id

--SELECT *
--FROM site
--WHERE site_id NOT IN (
--                    SELECT reservation.site_id
--                    FROM Reservations
--                    WHERE @DateStart >= DateStart AND @DateStart <= DateEnd
--                        AND @DateEnd >= DateStart AND DateEnd <= @DateEnd
--                )

--SELECT site.*, campground.*, reservation.*
--FROM site
--JOIN campground ON campground.campground_id = site.campground_id
--join reservation on reservation.site_id = site.site_id
--WHERE site_id NOT IN (
--                    SELECT reservation.site_id
--                    FROM reservation
--                    WHERE '2018-03-11' >= from_date AND '2018-03-11' <= to_date
--                        AND '2018-03-14' >= from_date AND to_date <= '2018-03-14'
--                )

--SELECT site.site_id, campground.name as campground, reservation.*
--FROM site
--JOIN campground ON campground.campground_id = site.campground_id
--join reservation on reservation.site_id = site.site_id
--WHERE DATEDIFF(DAY, '2018-0201', reservation.from_date) >= 1 AND DATEDIFF(DAY, '2018/02/28', reservation.from_date) >=1;

SELECT *
FROM reservation
WHERE (reservation.from_date Between GetDate() AND DATEADD(DAY, 30, GETDATE())) AND (reservation.to_date Between GetDate() AND DATEADD(DAY, 30, GETDATE())