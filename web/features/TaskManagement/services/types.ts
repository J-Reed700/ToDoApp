import { Priority, Status } from "../domain/types";

export interface CreateTaskRequest {
    categoryId: number;
    title: string;
    description?: string;
    priority: Priority;
    status: Status;
    dueDate?: string;
}

export interface CreateCategoryRequest {
    categoryName: string;
}

export interface CreateCommentRequest {
    taskId: number;
    comment: string;
}

export interface UpdateCommentRequest {
    id: number;
    comment: string;
}