export enum Priority {
    None = 0,
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

export enum Status {
    None = 0,
    InProgress = 1,
    Completed = 2,
    OnHold = 3,
    Cancelled = 4
}

export type TaskItem = {
    id: number;
    categoryId: number;
    title: string;
    description: string;
    status: Status;
    priority: Priority;
    dueDate: Date | string | null;
}

export type TaskComment = {
    id: number;
    comment: string;
    created: string; // DateTimeOffset from backend
    createdBy?: string;
    lastModified: string; // DateTimeOffset from backend  
    lastModifiedBy?: string;
}
  
export interface TaskCategory {
    id: number;
    categoryName: string;
    colour: string;
    tasks: TaskItem[];
}

