# Appendix: API Endpoint Blueprints

## GET /api/work-requests
1. Receive request → extract status, search, page, pageSize.  
2. Validate status filter → return 400 if invalid.  
3. Filter list by status + search text.  
4. Count total matches → `totalCount`.  
5. Paginate → slice one page based on page × pageSize.  
6. Convert to DTOs.  
7. Respond with `{ items, totalCount, page, pageSize }`.


## GET /api/work-requests/{id}
1. Receive `id` from URL.  
2. Find matching work request.  
   - If found → return 200 OK with DTO.  
   - If not found → return 404 Not Found.
     

## POST /api/work-requests
1. Receive new work request data (CreateWorkRequestDto).  
2. Validate required fields → auto 400 if missing.  
3. Build new `WorkRequest` object.  
4. Save to store.  
5. Return 201 Created with new item + Location header. 


## PATCH /api/work-requests/{id}/status
1. Receive `id` + new status.  
2. Find work request.  
   - If not found → return 404.  
3. Update `Status` + `UpdatedDate`.  
4. Return updated item.
   

## POST /api/work-requests/{id}/notes
1. Receive `id` + note text.  
2. Find work request.  
   - If not found → return 404.  
3. Update `Notes` field (overwrite).  
4. Update `UpdatedDate`.  
5. Return updated item.  

